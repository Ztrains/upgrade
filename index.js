const fs = require('fs'); // required to read https certs
const https = require('https');
const express = require('express');
const mongodb = require('mongodb');
const cookieParser = require('cookie-parser');
const bodyParser = require('body-parser');
const stormpath = require('express-stormpath');
const favicon = require('serve-favicon');
const bcrypt = require('bcryptjs');
const auth = require('./auth.js');
var app = express();

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: false
}));
app.use(auth.initialize());
app.use(auth.session());
app.use(cookieParser());
app.use(require('express-session')({ secret: 'This is a good secret', resave: false, saveUninitialized: false }));

app.use(favicon(__dirname + '/docs/favicon.ico'));

const port = process.env.PORT || 3000;
var db;
var users; //users collection;
var localTestUrl = 'mongodb://localhost:27017/test';

mongodb.MongoClient.connect(process.env.MONGODB_URI || localTestUrl, function(err, database) {
    if (err) {
        console.log('ERROR:', err);
        res.redirect('/')
    }

    // Save database object from the callback for reuse.
    db = database;
	users = database.collection("users");
	module.exports.users = users; //export users collection from module for use in auth.js
    console.log("Database connection ready");
});

//function logs in
app.post('/login', auth.authenticate('local', { failureRedirect: '/login' }),
  function(req, res) {
	if(!res.headersSent) {res.send('you have authenticated properly').redirect('/')};
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

app.post('/basic/test', auth.authenticate('basic', {session: false}), function(req, res) {
	console.log("basic/test succeeded for " + req.body.email);
	if(!res.headersSent) {res.send('Test auth succeeded');}
});


//registers a user in the user database
app.post('/reg', function(req, res) {
	users.findOne({email: req.body.email}, function(err, r) {
		if(err) {res.send("Database error");}
		else if(r) {res.send("User exists");}
		var salt = bcrypt.genSaltSync(10);
		var hash = bcrypt.hashSync(req.body.password, salt);
		users.insertOne({email: req.body.email, hash: hash}, function(err, r) {
			if(err) {res.send("Database error");}
			else {res.send("Success adding user");}

		});

	});

});

app.get('/', (req, res) => {
    res.send('Welcome to upgrade!');
});

app.get('/classList',  (req, res) => {
    //res.type('json');
    var list;
    db.collection('classes', (err, collection) => {
        if (err) {
            console.log('ERROR:', err);
            res.redirect('/')
        } else {
            collection.find({}, {
                _id: 1
            }).toArray((err, ret) => {
                if (err) {
                    console.log('ERROR:', err)
                    res.redirect('/')
                } else {
                    res.json(ret)
                }
            })
        }
    })
})

app.get('/join/:class/:type', (req, res) => {
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
                        name: req.user.fullName,
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

app.get('/name/:new', (req, res) => {
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
});
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

var server = app.listen(port);
