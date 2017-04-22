var users; //users collection

const admin = require('firebase-admin');
var serviceAccount = require('./upgradeKey.json');
admin.initializeApp({
  credential: admin.credential.cert(serviceAccount),
  databaseURL: "https://upgrade-fef84.firebaseio.com"
});

module.exports.addDevice = function(req, res) {
	if(!users) {users = require('./index.js').users;}
	if(!req.body.regKey) {
			console.log("Error no device regKey");
			res.status(400).send("Bad Request: no regKey");
			return;
	}
	console.log(req.user);
	users.updateOne({_id: req.user._id}, { $push: {devices: {regKey: req.body.regKey, date: new Date()}}}, function(err, r) {
		if(err) {
			console.log(err);
			res.send("Database error");
		} else {res.send("Success adding device");}
	});
};

module.exports.checkDevice = function(req, res) {
	if(!users) {users = require('./index.js').users;}
	if(!req.body.regKey) {
		console.log("Error no device regKey");
		res.status(400).send("Bad Request: no regKey");
	} else if(!req.body.regKey) {
		console.log("Error no device regKey");
		res.status(400).send("Bad Request: no regKey");
	} else {
		users.findOneAndUpdate({_id: req.user._id, "devices.regKey": req.body.regKey}, {$set: {"devices.$.date": new Date()}}, function(err, reg) {
			console.log(reg);
			if(err) {
				console.log("firebase.checkDevice find error:");
				console.log(err);
				res.send("Database error");
			} else if(reg.value) {
				res.send("Device key found");
			} else {
				res.send("Device key not found");
			}
		});
	}
};

module.exports.notifyDevices = function(user, chatID) {
	var tokens = [];
	for(var i = 0; i < user.devices.length; i++) {
		tokens.push(devices[i].regKey);
	}	
	admin.messaging().sendToDevice(tokens, chatID).then(function(res) {
		console.log("Success sending messages:", res);
	}).catch(function(err) {
		console.log("Error sending messages:", err);
	});
};
	
}
