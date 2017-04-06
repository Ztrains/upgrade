const admin = require('firebase-admin');
var users; //users collection
var serviceAccount = require('./upgradeKey.json');


admin.initializeApp({
  credential: admin.credential.cert(serviceAccount),
  databaseURL: "https://upgrade-fef84.firebaseio.com"
});

addDevice = function(req, res) {
	if(!users) {users = require('./index.js').users;}
	if(!req.body.regKey) {
			console.log("Error no device regKey");
			res.status(400).send("Bad Request: no regKey");
			return;
	}
	users.updateOne({_id: req.user._id}, { $push {devices: req.body.regKey}}, function(err, r) {
		if(!err) {
			console.log(err);
			res.send("Database error");
		} else {res.send("Success adding device");}
	}

};
