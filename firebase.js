var users; //users collection
var chats;

const admin = require('firebase-admin');
var serviceAccount = require('./upgradeKey.json');
admin.initializeApp({
  credential: admin.credential.cert(serviceAccount),
  databaseURL: "https://upgrade-fef84.firebaseio.com"
});

/*  Function to add a device to Firebase for notifications  */
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

/*  Function to check if a device is already registered */
module.exports.checkDevice = function(req, res) {
	if(!users) {users = require('./index.js').users;}
	if(!req.body.regKey) {
		console.log("Error no device regKey");
		res.status(400).send("Bad Request: no regKey");
	} else {
		users.findOneAndUpdate({_id: req.user._id, "devices.regKey": req.body.regKey}, {$set: {"devices.$.date": new Date()}}, function(err, reg) {
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

/*  Function to send the notification to a device   */
module.exports.notifyDevices = function(chatID, sender) {
	if(!users) {users = require('./index.js').users;}
	if(!chats) {chats = require('./index.js').chats;}
	chats.findOne({_id: chatID}, function(err, result) {
		if(err) {
			console.log("Database error in notifyDevices", err);
			return;
		}
		var tUsers = [];
		for(var i = 0; i < result.members.length; i++) {
			if(result.members[i].muted == false) {
				tUsers.push(result.members[i].user);
			}
		}
		var six = new Date();
		six.setMonth(six.getMonth() - 6);
		console.log("chat:", result);
		console.log("updating users:", tUsers);
		users.updateMany({_id: {$in: tUsers}}, {$pull: {devices: {date: {$lt: six}}}}, function(err) {
			if(err) {
				console.log("Database error removing old devices");
			}
			users.find({_id: {$in: tUsers}}, function(err, result) {
				result.forEach(function(user) {
					if(!user.devices || user == sender) {return;}
					console.log("Sending notification for user:,", user);
					var tokens = [];
					for(var i = 0; i < user.devices.length; i++) {
						tokens.push(user.devices[i].regKey);
					}
					console.log("Sending notifcation to the following devices:", tokens);
					var chatIDstr = chatID.toHexString();
					var payload = {data: {chatID: chatIDstr}};
					var options = {collapse_key: chatIDstr};
					admin.messaging().sendToDevice(tokens, payload, options).then(function(res) {
						console.log("Success sending messages:", res);
					}).catch(function(err) {
						console.log("Error sending messages:", err);
					});

				}, function(err) {
					if(err){
						console.log("Error in iterator");
						return;
					}
				});
			});
		});
	});
};
