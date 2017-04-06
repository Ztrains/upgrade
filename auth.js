/*used for authentication of login and sessions
Module used will be passport, because it will be easy to expand future
auth stragegies in the future.*/

const passport = require('passport');
const bcrypt = require('bcryptjs');
const LocalStrategy = require('passport-local').Strategy;
const BasicStrategy = require('passport-http').BasicStrategy;
var users; //users collection

module.exports = function(passport) {
    passport.use(new LocalStrategy(
        function(email, password, done) {
    		console.log("Local Strategy called");
    		if(!users) {users = require('./index.js').users;}
    		console.log("Autheticating email: %s", email);
            users.findOne( {email: email}, function (err, user) {
    			console.log("Err: " + err);
    			console.log("User: " + user);
              if(err)
                return  done(err);
              if(!user || !bcrypt.compareSync(password, user.hash))
                return done(null, false, {message: 'Incorrect email/password combo'});
    			console.log("Auth succeeded for user: " + user.email);
              done(null, user);
            });
    }));

    passport.serializeUser(function(user, cb) {
    	console.log("serialized called");
    	cb(null, user._id);
    });

    passport.deserializeUser(function(id, cb) {
    	console.log("deserialized called");
    	users.findOne( {_id: id}, function(err, user) {
    		if(err) { return cb(err);}
    		if(!user) {return cb(null, false); }
    		cb(null, user);
    	});
    });

    passport.use(new BasicStrategy(function(email, hash, done) {
    	console.log("Basic Strategy called");
    	if(!users) {users = require('./index.js').users;}
    	users.findOne( {email: email}, function(err, user) {
    		if(err) {return done(err);}
            if(!user || !(hash == user.hash)) {return done(null, false, {message: "bad return"});}
    		console.log("Basic auth succeeded for user: " + user.email);
    		done(null, user);
    	});
    }));
}


//module.exports = passport;
