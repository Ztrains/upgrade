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
var classes;    //classes collection
var localTestUrl = 'mongodb://localhost:27017/test';

/*  This is the process to connect to our MongoDB database hosted on mLab.
    we connect to the database and export the three documents into variables to use them later*/
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
    classes = database.collection("classes");
    module.exports.classes = classes;
    console.log("Database connection ready");
});

var localAuth = passport.authenticate('local');
var basicAuth = passport.authenticate('basic', {session: false});

/*  Route for logging in.  Code mostly in auth.js
    Sends a cookie to the user to show they're logged in.   */
app.post('/login', localAuth,
  function(req, res) {
	console.log('login successful');
	if(!res.headersSent) {res.send('you have authenticated properly')};
  });

/*  Route for logging out.
    Destroys the cookie the user has.
    User must be logged in to work, obviously.  */
app.post('/logout', function(req, res) {
	if(req.user) {
		req.logout()
		req.session.destroy();
		res.send('logout successfully');
	}
	//res.redirect('/login');
});

/*  Route to check if a user is logged in.
    Just used for debugging purposes, really.   */
app.post('/testlogin', function(req, res) {
	console.log('testing login: ',  req.cookies, '\nreq: ', req.session);
	if(req.user) { res.send('you are logged in as: ' + req.user.email);
	} else {res.send('login test failed');}
});

/*  Function gets salt for user for possible use with HTTP Basic authentication
    Not really used, as we use local authentication with cookies instead.   */
app.post('/basic/salt', function(req, res) {
	//return salt for requested user;
	users.findOne({email: req.body.email}, function(err, r) {
		if(err) {console.log("basic/salt err!!"); res.send("Database error");}
		else if(!r) {console.log("basic/salt user not found"); res.send("User doesn't exist");}
		console.log("result of salt search: " + r.email);
		res.json({email: r.email, salt: bcrypt.getSalt(r.hash)});//.send("Salt found");
	});
});

/*  Same idea as /testLogin but for basic authentication.
    Again, not really used since we use local auth with cookies.    */
app.post('/basic/test', basicAuth, function(req, res) {
	console.log("basic/test succeeded for " + req.body.email);
	if(!res.headersSent) {res.send('Test auth succeeded');}
});

/*  Registers a user in the users document.
    Requires an email and password, which gets salted.  */
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

/*  Registers device in database for use with Firebase notifications.
    JSON fields: "regKey"   */
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


/*  Checks if device is in database for Firebase notifications.
    JSON fields: "regKey"   */
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

/*  Possibly unneccessary, gets direct messages.
    JSON fields: N/A    */
app.post('/dms/get', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.getMessages(req, res);
});

/*  Basic authentication strategy to get messages.
    Unused, using local authentication. */
app.post('/basic/dms/get', basicAuth, function(req, res) {
	chat.getDms(req, res);
});

/*  Starts a new dm with a user
    JSON fields: "dm_user" (user id of target user) */
app.post('/dms/start', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.startDM(req, res);
});

/*  Starts a new message board for a class.
    JSON fields: "classID" (name of class)  */
app.post('/dms/class/start', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.startClassDM(req, res);
});

/*  Basic authentication to start new DM.
    Unused, again.  */
app.post('/basic/dms/start', basicAuth, function(req, res) {
	chat.startDM(req, res);
});

/*  Sends a message in specified chat
    JSON fields: "chatID", "message" (just payload, no timestamp or sender info)    */
app.post('/chat/message/send', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.sendMessage(req, res);
});

/*  Sends a message in the specified class message board.
    JSON fields: "chatID", "message" (just payload, no timestamp or sender info), "classID" (name of class to message)  */
app.post('/chat/class/message/send', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.sendClassMessage(req, res);
});

/*  Basic authentication to send message.
    Unused.  */
app.post('/basic/chat/message/send', basicAuth, function(req ,res) {
	chat.sendMessage(req, res);
});

/*  Get messages in specified chat
    JSON fields: "chatID", "start"(optional, start of message range), "end" (optional, end of message range)    */
app.post('/chat/messages/get', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.getMessages(req, res);
});

/*  Basic authentication to get messages.
    Unused. */
app.post('/basic/chat/message/get', basicAuth, function(req, res) {
	chat.getMessages(req, res);
});

/*  Get message count in specified chat
    JSON fields: "chatID"   */
app.post('/chat/message/count', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	chat.getMessageCount(req, res);
});

/*  Basic authentication strategy to get messages.
    Unused. */
app.post('/basic/chat/message/count', basicAuth, function(req, res) {
	chat.getMessageCount(req, res);
});

/*  Home route when going to our URL in a browser.
    Unused, was just for testing purposes.  */
app.get('/', (req, res) => {
    res.sendFile(__dirname + '/index.html');
});

/*  Route to retrieve list of classes stored in the classes document.
    JSON fields: None, it's a GET.  */
app.get('/classList',  (req, res) => {
    db.collection('classes', (err, collection) => {
        if (err) {
            console.log('ERROR:', err);
            res.redirect('/')
        } else {
            collection.distinct('name', {}, {}, (err, result)=>{
                res.json({"classes": result})
            })
        }
    })
})

/*  Route to add a new class to the classes document.
    JSON fields: "className" (name of class to add) */
app.post('/newClass', (req,res)=>{
    if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
    classes.insertOne({name: req.body.className}, function(err, r) {
        if(err) {res.send("Could not create new class")}
        else {res.send("Success adding class")}

    });
})

/*  Route to add a user to a class.  Adds them in both the classes document under the correct class, and adds to their profile.
    JSON fields: "className" (name of class to add student to), "type" (type user will be for a class e.g. 'student' or 'tutor')    */
app.post('/joinClass', (req,res)=>{
    if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
    if(!req.user.name) {
        console.log("ERROR: User needs a name")
        res.send('Did not add user to class.')
        return;
    }
    db.collection('classes', (err, collection) => {
        if (err) {
            console.log('ERROR:', err);
            res.redirect('/')
        } else {
            collection.update({
                name: req.body.className
            }, {
                $addToSet: {
                    students: {
                        name: req.user.name,
                        type: req.body.type
                    }
                }
            })
        }
    })
    users.findOneAndUpdate(
        {"name":req.user.name},
        {$addToSet: {
            classesIn: {
                className: req.body.className,
                type: req.body.type
            }
        }
    })
    console.log("added " + req.user.name + " as a " + req.body.type + " to class " + req.body.className)
    res.send("user added")
})

/*  Route to remove a user from a class.  Removes them from the array of students under the document.
    JSON fields: "className" (name of class to add student to), "type" (type user will be for a class e.g. 'student' or 'tutor')    */
app.post('/leaveClass', (req,res)=>{
    if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
    console.log('req.body.className: ' + req.body.className)
    console.log('req.body.type: ' + req.body.type)
    db.collection('classes', (err, collection) => {
        if (err) {
            console.log('ERROR:', err);
            res.send('Error removing from class')
        } else {
            collection.update({
                name: req.body.className
            }, {
                $pull: {
                    students: {
                        name: req.user.name,
                        type: req.body.type
                    }
                }
            })
        }
    })
    users.findOneAndUpdate(
        {"name":req.user.name},
        {$pull: {
            classesIn: {
                className: req.body.className,
                type: req.body.type
            }
        }
    })
    console.log("removed " + req.user.name + " as a " + req.body.type + " from class " + req.body.className)
    res.send('user removed')
})

/*  Retrieves everything in a user's profile and sends the entire profile in JSON.
    JSON fields: "name" (name of user), "email" (email of user) */
app.post('/retrieveProfile', (req, res)=>{
    console.log('name trying to retrieve: ' + req.body.name)
    console.log('email trying to retrieve: ' + req.body.email)
    users.findOne({$or: [{name: req.body.name}, {email: req.body.email}]}, function(err, profile) {
		if(err) {console.log("Retrieval error"); return res.send("retrieval error");}
		else if(!profile) {console.log("email not found"); return res.send("User doesn't exist/Email not found");}
		console.log("Profile retrieved: " + JSON.stringify(profile,null,2));  //Makes the JSON 'pretty'
        res.type('json');
		res.json(profile);    //Client will parse it all
	});
})

/*  Route to retrieve a profile for looking at right after login.
    Pretty sure it's not used anymore, basically just a clone of the above route.
    JSON fields: "name" (name of user), "email" (email of user) */
app.post('/retrieveLogin', (req,res)=>{
    console.log('login profile email: ' + req.body.email)
    users.findOne({email: req.body.email}, function(err, profile) {
		if(err) {console.log("Retrieval error"); return res.send("retrieval error");}
		else if(!profile) {console.log("email not found"); return res.send("User doesn't exist/Email not found");}
		console.log("Profile retrieved: " + JSON.stringify(profile,null,2));
        res.type('json');
		res.json(profile);
	});
})

/*  Route to update a user's profile.  Checks if certain fields exists and updates them if something was sent in req.
    JSON fields required: "email" (email of user to update)
    JSON fields possible:   "name" (name of user), "newemail" (new email to be stored), "contact" (contact info), "about" (about info),
                            "tutor" (field to show if a tutor, possibly unused), "student" (field to show if a student, possibly unused),
                            "time" (times available), "price" (pricing options), "avatar" (URL of avatar displayed),
                            "visible" (field if profile is private or not), "admin" (field if user is admin, probably won't be used by client)  */
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
        if (data.avatar) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"avatar":data.avatar}}
            )
            console.log("avatar updated to: " + data.avatar)
        }
        if (data.visible) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"visible":data.visible}}
            )
            console.log("visible updated to: " + data.visible)
        }
        if (data.admin) {
            users.findOneAndUpdate(
                {"email":data.email},
                { $set: {"admin":data.admin}}
            )
            console.log("admin updated to: " + data.admin)
        }
	});
    res.send("profile updated")
})

/*  Route to list all users in a specified class.
    JSON fields: "className" (name of class)    */
app.post('/studentsInClass', (req,res)=>{
    //res.type('json')
    db.collection('classes', (err, collection)=>{
        if (err) {
            console.log('ERROR:', err);
            res.redirect('/')
        }
        else {
            collection.find({name: req.body.className},{students:1, _id:0}).toArray(function(err, listofstudents) {
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

/*  Route for upvoting or downvoting a user.
    JSON fields:    "email" (email of user doing upvoting, probably unneeded), "vote" (should be 'up' or 'down', indicating upvote or downvote),
                    "name" (name of user to upvote/downvote)    */
app.post('/upvote', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
        if (err) {
            console.log("ERROR: " + err);
            res.send(1);
        }
        if (req.body.vote == 'up') {
            users.findOneAndUpdate(
                {"name":req.body.name},
                { $inc: {"rating":1}}
            )

            console.log("rating raised by 1")

        }
        else if (req.body.vote == 'down') {
            users.findOneAndUpdate(
                {"name":req.body.name},
                { $inc: {"rating":-1}}
            )
            console.log("rating lowered by 1")
        }
        else {
            console.log("must send 'up' or 'down' in vote field")
            return res.send("invalid vote, must be up or down")
        }
	});
    users.findOneAndUpdate(
        {"name":req.body.name},
        {$addToSet: {
            usersUpvoted: {
                _id: req.user._id.toString()
            }
        }
    })
    res.send("rating updated")
})

/*  Route for setting recovery question/answer for a user.
    Unused now, new password updating method used.
    JSON fields: "email" (email of user), "question" (recovery question), "answer" (recovery answer)    */
app.post('/setRecovery', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
        if (err) {
            console.log("ERROR: " + err);
            res.send(1);
        }
        var data = req.body
        users.findOneAndUpdate(
            {"email":req.body.email},
            { $set: {"question":req.body.question, "answer":req.body.answer}}
        )
        console.log("Security question: " + req.body.question)
        console.log("Question Answer: " + req.body.answer)
	});
    res.send("recovery set")
})

/*  Sends the recovery question to the user so they can change their password.
    Unused, new strategy used instead.
    JSON fields: "email" (email of user)    */
app.post('/getQuestion', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, r) {
		if(err) {console.log("Could not find email"); res.send("Database error");}
		else if(!r) {console.log("user not found"); res.send("User doesn't exist");}
		console.log("question returned: " + r.question);
		res.json({question: r.question});
	});
})

/*  Route to do the password resetting of the user.
    Unused, new strategy used instead.
    JSON fields: "email" (email of user), "answer" (answer sent by user)    */
app.post('/doRecovery', (req,res)=>{
    users.findOne({email: req.body.email}, function(err, profile) {
        if (err) {
            console.log("ERROR: " + err);
            res.send(1);
        }
        var data = req.body
        var ans = req.user.answer

        if (req.body.answer == ans) {
            console.log("updating password happens here")
            res.send("Password change successful")
        }
        else {
            console.log("wrong answer sent")
            res.send("Incorrect answer")
        }
	});
})
/* Routhe to change the password of a user
 * JSON fields: "password", "newpassword" */
app.post('/changePassword', function(req, res) {
	if(!req.user) {
		res.status(401).send("Not logged in");
		return;
	}
	auth.changePassword(req, res);
});
app.post('/basic/changePassword', basicAuth, function(req, res) {
	auth.changePassword(req, res);
});

/* Route to generate a new password
 * JSON fields: "email" */
app.post('/resetPassword', function(req, res) {
	auth.resetPassword(req, res);
});

/*  Route to block a user.
    JSON fields: "id" (_id of user being blocked)  */
app.post('/blockUser', (req,res)=>{
    users.findOneAndUpdate(
        {"email":req.user.email},
        {$addToSet: {
            blockedUsers: {
                id: req.body.id,
            }
        }}
    )
    console.log("User with id " + req.body.id + " has been blocked by user with id " + req.user._id.toString())
    res.send("User blocked")
})

/*  Route to unblock a user.
    JSON fields: "id" (_id of user being unblocked)  */
app.post('/unblockUser', (req,res)=>{
    users.findOneAndUpdate(
        {"email":req.user.email},
        {$pull: {
            blockedUsers: {
                id: req.body.id,
            }
        }}
    )
    console.log("User with id " + req.body.id + " has been unblocked by user with id " + req.user._id.toString())
    res.send("User unblocked")
})

/*  Route to report a user.
    JSON fields: "id" (_id of user being reported), "reason" (reason given for why they are being reported)  */
app.post('/reportUser', (req,res)=>{
    classes.findOneAndUpdate(
        {"_id":"reports"},
        {$addToSet: {
            reportedUsers: {
                id: req.body.id,
                reason: req.body.reason,
                name: req.body.name
            }
        }}
    )
    console.log("User with id " + req.body.id + " has been reported.")
    console.log("Reason given: " + req.body.reason)
    res.send("User reported")
})

/*  Route to get all of the reports made.
    JSON fields: N/A    */
app.post('/getReports',(req,res)=>{
    db.collection('classes', (err, collection) => {
        if (err) {
            console.log('ERROR:', err);
            res.send("error")
            return
        } else {
            collection.distinct('reportedUsers', {}, {}, (err, result)=>{
                res.json({"reportedUsers": result})
            })
        }
    })
})

/*  Starts the server with the port to listen on.   */
http.listen(port, ()=>{
    console.log("listening on " + port)
});
