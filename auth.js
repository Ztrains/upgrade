/*used for authentication of login and sessions
Module used will be passport, because it will be easy to expand future
auth stragegies in the future.*/

const passport = require('passport');
const bcrypt = require('bcryptjs');
const LocalStrategy = require('passport-local').Strategy);

passport.use(new LocalStrategy(
    function(email, password, users, done) {
        users.find( {email: email}, function (err, user) {
          if(err) 
            return  done(err);
          if(!user || !bcrypt.compareSync(password, user.hash))
            return done(null, false, {message: 'Incorrect email/password combo'});
          done(null, user)
        });
    });
);
