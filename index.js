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
const auth = require('./auth.js');
const firebase = require('./firebase.js');

var app = express();
const http = require('http').Server(app)
var chat = require('./chat.js')
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
var chats; //chats collection;
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
	chats = database.collection("chats");
	module.exports.chats = chats;
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
		console.log(r);
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
//register Device in Database
//JSON fields: "regKey"
app.post('/regDevice', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	firebase.addDevice(req, res);
});
app.post('/basic/regDevice', basicAuth, function(req, res) {
	firebase.addDevice(req, res);
});


//check device in database
//JSON fields: "regKey"
app.post('/checkDevice', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	firebase.checkDevice(req, res);
});
app.post('/basic/checkDevice', basicAuth, function(req, res) {
	firebase.checkDevice(req, res);
});

//possibly unneccessary
//get direct messages;
//JSON fields: N/A
app.post('/dms/get', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.getDms(req, res);
});
app.post('/basic/dms/get', basicAuth, function(req, res) {
	chat.getDms(req, res);
});
//start a dm with a new user
//JSON fields: "dm_user" (user id of target user)
app.post('/dms/start', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.startDM(req, res);
});
app.post('/basic/dms/start', basicAuth, function(req, res) {
	chat.startDM(req, res);
});

//send a message in specified chat
//JSON fields: "chatID", "message" (just payload, no timestamp or sender info)
app.post('/chat/sendMessage', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.sendMessage(req, res);
});
app.post('/basic/chat/sendMessage', basicAuth, function(req ,res) {
	chat.sendMessage(req, res);
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

app.post('/retrieveProfile', (req, res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {     //make an $or with name
		if(err) {console.log("Retrieval error"); return res.send("retrieval error");}
		else if(!profile) {console.log("email not found"); return res.send("User doesn't exist/Email not found");}
		console.log("result of salt search: " + JSON.stringify(profile,null,2));  //should log everything in the profile in theory
        res.type('json');
		res.json(profile);    //up to client to parse i guess lol sorry
	});
})

app.post('/updateProfile', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
        if (err) {
            console.log("ERROR: " + err);
            res.send(1);
        }
        console.log('Profile found =' + JSON.stringify(profile))
        var data = req.body;
        console.log('Full update request =' + JSON.stringify(data))

        if (data.name) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"name":data.name}}
            )
            console.log("name updated to: " + data.name)
        }
        if (data.newemail) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"email":data.newemail}}
            )
            console.log("email updated to: " + data.newemail)
        }
        if (data.contact) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"contact":data.contact}}
            )
            console.log("contact updated to: " + data.contact)
        }
        if (data.about) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"about":data.about}}
            )
            console.log("about updated to: " + data.about)
        }
        if (data.tutor) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"tutor":data.tutor}}
            )
            console.log("tutor updated to: " + data.tutor)
        }
        if (data.student) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"student":data.student}}
            )
            console.log("student updated to: " + data.student)
        }
        if (data.time) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"time":data.time}}
            )
            console.log("time updated to: " + data.time)
        }
        if (data.price) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"price":data.price}}
            )
            console.log("price updated to: " + data.price)
        }
	});
    res.send("profile updated")
})

app.post('/studentsInClass', (req,res)=>{
    //res.type('json')
    db.collection('classes', (err, collection)=>{
        if (err) {
            console.log('ERROR:', err);
            res.redirect('/')
        }
        else {
            collection.find({_id: req.body.className},{students:1, _id:0}).toArray(function(err, listofstudents) {
                if (err) {
                    console.log('ERROR:', err);
                    res.redirect('/')
                }
                else {
                    //console.log("listofstudents= " + JSON.stringify(listofstudents));
                    listofstudents = listofstudents[0]
                    res.json(listofstudents)
                }
            })
        }
    })
})

app.post('/upvote', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
        if (err) {
            console.log("ERROR: " + err);
            res.send(1);
        }
        if (req.body.vote == 'up') {
            users.findOneAndUpdate(
                {"email":req.body.email},
                { $inc: {"rating":1}}
            )
            console.log("rating raised by 1")
        }
        else if (req.body.vote == 'down') {
            users.findOneAndUpdate(
                {"email":req.body.email},
                { $inc: {"rating":-1}}
            )
            console.log("rating lowered by 1")
        }
        else {
            console.log("must send 'up' or 'down' in vote field")
            return res.send("invalid vote, must be up or down")
        }
	});
    res.send("rating updated")
})

app.post('/setRecovery', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
        if (err) {
            console.log("ERROR: " + err);
            res.send(1);
        }
        var data = req.body
        //if (req.body.vote == 'up') {
            users.findOneAndUpdate(
                {"email":req.body.email},
                { $set: {"question":req.body.question, "answer":req.body.answer}}
            )
            console.log("Security question: " + req.body.question)
            console.log("Question Answer: " + req.body.answer)
        //}
	});
    res.send("recovery set")
})

app.post('/getQuestion', (req,res)=>{
    users.find({email: req.body.email},{question:1, _id:0}, function(err, ret) {
        if (err) {
            res.send("Email does not exist")
        }
        console.log('sending question: ' + ret )
        res.json(ret)
	});
    //res.send("recovery set")
})

app.post('/doRecovery', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
        if (err) {
            console.log("ERROR: " + err);
            res.send(1);
        }
        var data = req.body
        var ans = profile.answer

        if (req.body.answer == ans) {
            console.log("updating password happens here")
            res.send("Password change successful")
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
