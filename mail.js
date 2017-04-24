const mailer = require('nodemailer');

var resetTransport = mailer.createTransport({
	service: 'gmail',
	auth: {user: 'upgradereset@gmail.com', pass: 'iamtootiredforthisshit'}
});

/*	Function to email users a new password.  Unsure if used.	*/
module.exports.sendReset = function(user, password) {
	var mailOptions = {
		from: 'upgradereset@gmail.com',
		to: user.email,
		subject: 'Upgrade password reset',
		text: 'You have reset you password. A random one has been generate for you. Please change it immediatly: ' + password,
		html: 'You have reset you password. A random one has been generate for you. Please change it immediatly: <b>' + password + '</b>'
	}
	resetTransport.sendMail(mailOptions, function(err, info) {
		if(err) {
			console.log('reset password email error');
			console.log(err);
		}
	});
}
