var users; //users collection
var chats;

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

module.exports.notifyDevices = function(chatID) {
	if(!users) {users = require('./index.js').users;}
	if(!chats) {chats = require('./index.js').chats;}
	chats.findOne({_id: chatID}, function(err, result) {
		if(err) {
			console.log("Database error in notifyDevices", err);
			return;
		}
		var tUsers = [];
		for(var i = 0; i < result.members.length; i++) {
			if(result.members[i].muted != false) {
				tUsers.push(result.members[i].user);	
			}	
		}
		for(var i = 0; i < users.length; i++) {
			if(err) {
				console.log("Database error in notifyDevices", err);
				return;
			}
			users.findOne({_id: tUsers[i]}, function(err, result) {
				var tokens = [];
				for(var n = 0; n < result.devices.length; n++) {
					tokens.push(result.devices[i].regKey);
				}
				admin.messaging().sendToDevice(tokens, chatID).then(function(res) {
					console.log("Success sending messages:", res);
				}).catch(function(err) {
					console.log("Error sending messages:", err);
				});
			});
		}
	});
};
