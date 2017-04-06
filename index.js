const fs = require('fs'); // required to read https certs
const https = require('https');
const passport = require('passport');
const express = require('express');
const mongodb = require('mongodb');
const cookieParser = require('cookie-parser');
const bodyParser = require('body-parser');
//const stormpath = require('express-stormpath');  RIP Stormpath
const favicon = require('serve-favicon');
const bcrypt = require('bcryptjs');
const auth = require('./auth.js')(passport);

var app = express();
const http = require('http').Server(app)
//var chat = require('./chat.js')
var io = require('socket.io')(http);

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: false
}));
app.use(require('express-session')({ secret: 'This is a good secret', resave: false, saveUninitialized: false, cookie: {secure: false, maxAge: 3600000}}));
app.use(passport.initialize());
app.use(passport.session());
app.use(cookieParser());

app.use(favicon(__dirname + '/docs/favicon.ico'));

const port = process.env.PORT || 3000;
var db;
var users; //users collection;
var localTestUrl = 'mongodb://localhost:27017/test';


mongodb.MongoClient.connect(process.env.MONGODB_URI || localTestUrl, function(err, database) {
    if (err) {
        console.log('ERROR:', err);
        res.redirect('/');
    }

    // Save database object from the callback for reuse.
    db = database;
	users = database.collection("users");
	module.exports.users = users; //export users collection from module for use in auth.js
    console.log("Database connection ready");
});

var localAuth = passport.authenticate('local');
var basicAuth = passport.authenticate('basic', {session: false});

//function logs in
app.post('/login', localAuth,
  function(req, res) {
	console.log('login successful');
	if(!res.headersSent) {res.send('you have authenticated properly')};
  });

app.post('/logout', function(req, res) {
	if(req.user) {
		req.logout()
		req.session.destory();
		res.send('logout successfully');
	}
	//res.redirect('/login');
});

app.post('/testlogin', function(req, res) {
	console.log('testing login: ',  req.cookies, '\nreq: ', req.session);
	if(req.user) { res.send('you are logged in as: ' + req.user.email);
	} else {res.send('login test failed');}
});

//function gets salt for user for possible use with HTTP Basic authentication
app.post('/basic/salt', function(req, res) {
	//return salt for requested user;
	users.findOne({email: req.body.email}, function(err, r) {
		if(err) {console.log("basic/salt err!!"); res.send("Database error");}
		else if(!r) {console.log("basic/salt user not found"); res.send("User doesn't exist");}
		console.log("result of salt search: " + r.email);
		res.json({email: r.email, salt: bcrypt.getSalt(r.hash)});//.send("Salt found");
	});
});

app.post('/basic/test', basicAuth, function(req, res) {
	console.log("basic/test succeeded for " + req.body.email);
	if(!res.headersSent) {res.send('Test auth succeeded');}
});


//registers a user in the user database
app.post('/reg', function(req, res) {
	users.findOne({email: req.body.email}, function(err, r) {
		if(err) {res.send("Database error");}
		else if(r) {res.send("User exists"); return;}
		var salt = bcrypt.genSaltSync(10);
		var hash = bcrypt.hashSync(req.body.password, salt);
		users.insertOne({email: req.body.email, hash: hash}, function(err, r) {
			if(err) {res.send("Database error");}
			else {res.send("Success adding user");}

		});

	});

});


app.get('/', (req, res) => {
    //res.send('Welcome to upgrade!');
    res.sendFile(__dirname + '/index.html');
});

app.get('/classList',  (req, res) => {
    res.type('json');
    var list=[];
    db.collection('classes', (err, collection) => {
        if (err) {
            console.log('ERROR:', err);
            res.redirect('/')
        } else {
            /*collection.find({}, {
                _id: 1
            }).forEach((err, doc) => {
                if (err) {
                    console.log('ERROR:', err)
                    res.redirect('/')
                } else {
                    list.push(doc)
                }
            })*/
            collection.distinct('_id', {}, {}, (err, result)=>{
                //res.send(JSON.stringify({classes: result})
                res.json({"classes": result})
            })
        }
    })
    //res.json(list);
})

app.get('/join/:class/:type/:name', (req, res) => {     //TODO: instead of :name use from database later
    var list;
    db.collection('classes', (err, collection) => {
        if (err) {
            console.log('ERROR:', err);
            res.redirect('/')
        } else {
            collection.update({
                _id: req.params.class
            }, {
                $push: {
                    students: {
                        name: req.params.name,
                        type: req.params.type
                    }
                }
            })
        }
    })

    //db.classes.update({_id: req.params.class}, {$pull: {students: req.user.userName}})

    //res.send('Hello, ' + req.user.givenName + ', your username is ' + req.params.username)
    res.redirect('/')
})

/*app.get('/name/:new', (req, res) => {
    req.user.givenName = req.params.new;
    req.user.save(function(err) {
        if (err) {
            res.status(400).end('Oops!  There was an error: ' + err.userMessage);
        } else {
            res.end('Name was changed!');
        }
    });
});

app.get('/email/:new', (req, res) => {
    req.user.email = req.params.new;
    req.user.username = req.params.new;
    req.user.save(function(err) {
        if (err) {
            res.status(400).end('Oops!  There was an error: ' + err.userMessage);
        } else {
            res.end('Email/Username was changed!');
        }
    });
});*/

app.get('/info/:type/:val', (req, res) => {
    req.user.customData[req.params.type] = req.params.val;

    req.user.customData.save(function(err) {
        if (err) {
            res.status(400).end('Oops!  There was an error: ' + err.userMessage);
        } else {
            res.end('Custom Data added!');
        }
    });
});

app.get('/retrieveProfile', (req, res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
		if(err) {console.log("Retrieval error"); res.send("retrieval error");}
		else if(!profile) {console.log("email not found"); res.send("User doesn't exist/Email not found");}
		console.log("result of salt search: " + JSON.stringify(JSON.parse(profile),null,2));  //should log everything in the profile in theory
		res.json(profile);    //up to client to parse i guess lol sorry
	});
})

app.post('/updateProfile', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
		var data = req.body;
        console.log('Full update request =' + JSON.stringify(data))
        if (data.name) {
            collection.update({'name': data.name})
        }
        if (data.email) {
            collection.update({'email': data.email})
        }
        if (data.contact) {
            collection.update({'name': data.contact})
        }
        if (data.about) {
            collection.update({'name': data.about})
        }
        if (data.tutor) {
            collection.update({'name': data.tutor})
        }
        if (data.student) {
            collection.update({'name': data.student})
        }
        if (data.time) {
            collection.update({'name': data.time})
        }
        if (data.price) {
            collection.update({'name': data.price})
        }
	});
})

http.listen(port, ()=>{
    console.log("listening on " + port)
});


/*****************************************************
**************This is staying in for now**************
*****************************************************/

var usernames = {};

function check_if_exists(id) {
	for (var name in usernames) {
		if (name === id) {
			return true;
		}
	}
	return false;
}

io.on("connection", (client)=>{
	console.log('user connected')

	client.on('adduser', (username)=>{
		//store username in socket session for this client
		client.username = username;
		//add clients username to global list
		if (check_if_exists(username) === false)
			usernames[username] = client.id;
        console.log("*** Usernames ***")
        for (var name in usernames) {
    		console.log('\t Name: ' + name)
    	}
	});

	// when the user sends a private message to a user.. perform this
	client.on('msg_user', function(user_to, user_from, msg) {
		console.log("From user: "+user_from);
		console.log("To user: "+user_to);
		//console.log(usernames);
		io.sockets.client(usernames[user_to]).emit('updatechat', msg);

	});
});

/*****************************************************
**********************End of chat*********************
*****************************************************/
