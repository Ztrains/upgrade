/*used for authentication of login and sessions
Module used will be passport, because it will be easy to expand future
auth stragegies in the future.*/

const passport = require('passport');
const bcrypt = require('bcryptjs');
const LocalStrategy = require('passport-local').Strategy;
const BasicStrategy = require('passport-http').BasicStrategy;
var users; //users collection

passport.use(new LocalStrategy(
    function(email, password, done) {
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
	cb(null, user.email);
});

passport.deserializeUser(function(email, cb) {
	users.find( {email: email}, function(err, user) {
		if(err) { return cb(err);}
		if(!user) {return cb(null, false); }
		cb(null, user);
	});
});

passport.use(new BasicStrategy(function(email, hash, done) {
	if(!users) {users = require('./index.js').users;}
	users.findOne( {email: email}, function(err, user) {
		if(err) {return done(err);}
        if(!user || !(hash == user.hash)) {return done(null, false, {message: "bad return"});}
		console.log("Basic auth succeeded for user: " + user.email);
		done(null, user);
	});
}));


module.exports = passport;
module.exports.authLocal = passport.authenticate('local', { failureRedirect: '/login'});
module.exports.authBasic = passport.authenticate('basic', { session: false});
