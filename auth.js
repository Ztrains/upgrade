/*used for authentication of login and sessions
Module used will be passport, because it will be easy to expand future
auth stragegies in the future.*/

const passport = require('passport');
const bcrypt = require('bcryptjs');
const LocalStrategy = require('passport-local').Strategy;
const BasicStrategy = require('passport-http').BasicStrategy;
const objectID = require('mongodb').ObjectID
const mail = require('./mail.js');
var users; //users collection

passport.use(new LocalStrategy({usernameField: "email"},
function(email, password, done) {
	console.log("Local Strategy called");
    	if(!users) {users = require('./index.js').users;}
    	console.log("Autheticating email: %s", email);
       	users.findOne( {email: email}, function (err, user) {
    		console.log("Err: " + err);
    		console.log("User: " + user);
        	if(err) {
				console.log("Error in Local Auth find function:");
				console.log(err);
         		return  done(err);
			}
        	if(!user || !bcrypt.compareSync(password, user.hash))
			return done(null, false, {message: 'Incorrect email/password combo'});
		console.log("Auth succeeded for user: " + user.email);
		done(null, user);
	});
}));

passport.serializeUser(function(user, cb) {
    	cb(null, user._id);
});

passport.deserializeUser(function(id, cb) {
		console.log("deserialize function");
		console.log(id);
    	users.findOne( {_id: new objectID(id)}, function(err, user) {
    		if(err) { return cb(err);}
    		if(!user) {return cb(null, false); }
    		cb(null, user);
    	});
});

passport.use(new BasicStrategy({usernameField: "email"}, function(email, hash, done) {
    	if(!users) {users = require('./index.js').users;}
    	users.findOne( {email: email}, function(err, user) {
    		if(err) {
				console.log("Error in Baic Auth find function:")
				console.log(err);
				return done(err);}
			if(!user || !(hash == user.hash)) {return done(null, false, {message: "bad return"});}
    		console.log("Basic auth succeeded for user: " + user.email);
    		done(null, user);
  	});
}));


module.exports = passport;
module.exports.changePassword = function(req, res) {
    if(!users) {users = require('./index.js').users;}
	if(!req.body.email) {
		console.log("Bad Request: missing email key");
		res.status(400).send("Bad Request: missing email key");
		return;
	}
	if(!req.body.password) {
		console.log("Bad Request: missing password key");
		res.status(400).send("Bad Request: missing password key");
		return;
	}
	if(!req.body.newpassword) {
		console.log("Bad Request: missing newpassword key");
		res.status(400).send("Bad Request: missing newpassword key");
		return;
	}
	users.findOne({email: req.body.email}, function(err, user) {
		if(err) {
			console.log("auth.changePassword: Database error");
			console.log(err);
			res.status(500).send("Database error occurred!");
			return;
		}
		if(!user) {
			console.log("auth.changePassword: user not found");
			res.status(401).send("user not found");
			return;
		}
		var salt = bcrypt.genSaltSync(10);
		var hash = bcrypt.hashSync(req.body.newpassword, salt);
		users.findOneAndUpdate({_id: user._id}, {$set: {hash: hash}}, function(err, result) {
			if(err) {
				console.log("auth changePassword")
				console.log(err);
				res.status(500).send("Database error occurred");
			} else if(!result.value) {
				console.log("auth.changePassword: Database error");
				res.status(500).send("Database error occurred");
			} else {
				res.send("Password changed");
			}
		});

	});
};

module.exports.resetPassword = function(req, res) {
    if(!users) {users = require('./index.js').users;}
	if(!req.body.email) {
		console.log("Bad Request: missing email key");
		res.status(400).send("Bad Request: missing email key");
		return;
	}
	users.findOne({email: req.body.email}, function(err, user) {
		if(err) {
			console.log("auth.changePassword: Database error");
			console.log(err);
			res.status(500).send("Database error occurred!");
			return;
		}
		if(!user) {
			console.log("auth.resetPassword: user not found");
			res.status(401).send("User not found");
			return;
		}
		require('crypto').randomBytes(5, function(err, buffer) {
  			var token = buffer.toString('base64');
			var salt = bcrypt.genSaltSync(10);
			var hash = bcrypt.hashSync(token, salt);
			users.findOneAndUpdate({_id: user._id}, {$set: {hash: hash}}, function(err, result) {
				if(err) {
					console.log("auth changePassword")
					console.log(err);
					res.status(500).send("Database error occurred");
				} else if(!result.value) {
					console.log("auth.changePassword: Database error");
					res.status(500).send("Database error occurred");
				} else {
					mail.sendReset(user, token);
					res.send("Password changed. Email sent");
				}
			});

		});
	});
};
