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

/*	Local authentication for users.	*/
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

/*	Function which serializes user profiles	*/
passport.serializeUser(function(user, cb) {
    	cb(null, user._id);
});

/*	Function which deserializes users	*/
passport.deserializeUser(function(id, cb) {
		console.log("deserialize function");
		console.log("user: " + id);
    	users.findOne( {_id: new objectID(id)}, function(err, user) {
    		if(err) { return cb(err);}
    		if(!user) {return cb(null, false); }
    		cb(null, user);
    	});
});

/*	Basic authentication strategy.  Unused.	*/
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

/*	Function for password changing	*/
module.exports = passport;
module.exports.changePassword = function(req, res) {
    if(!users) {users = require('./index.js').users;}

	console.log('req.body.recovered: ' + req.body.recovered)
	if(req.body.recovered) {
		console.log('Resetting password via recovery questions')
		//Should skip the checks maybe, should be changed
	}
	else if(!req.body.password) {
		console.log("Bad Request: missing password key");
		res.status(400).send("Bad Request: missing password key");
		return;
	}
	else if(!req.body.newpassword) {
		console.log("Bad Request: missing newpassword key");
		res.status(400).send("Bad Request: missing newpassword key");
		return;
	}
	else if(!bcrypt.compareSync(req.body.password, req.user.hash)) {
		console.log('wrong original password')
		res.status(401).send("Wrong original password");
		return;
	}
	var salt = bcrypt.genSaltSync(10);
	var hash = bcrypt.hashSync(req.body.newpassword, salt);
	users.findOneAndUpdate({email: req.body.email}, {$set: {hash: hash}}, function(err, result) {
		if(err) {
			console.log(err);
			res.status(500).send("Database error occurred");
		} else if(!result.value) {
			console.log("auth.changePassword: Database error");
			res.status(500).send("Database error occurred");
		} else {
			res.send("Password changed");
		}
	});
};

/*	Additional function for password resetting	*/
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
			res.send("If user exists, a new password will be sent by email.");
			return;
		}
		require('crypto').randomBytes(12, function(err, buffer) {
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
					res.send("If user exists, a new password will be sent by email.");
				}
			});

		});
	});
};
