const objectID = require('mongodb').ObjectID
const firebase = require('./firebase.js');
var users;
var chats;
var classes;

/*	Function for gettings DMs of a user.  Unused.	*/
module.exports.getDms= function(req, res) {
	res.json({dms: req.user.dms});
}

/*	Function for sending a message.	*/
module.exports.sendMessage = function (req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}

	if(!req.body.chatID) {
		res.status(400).send("Bad Request: missing chatID key");
		return;
	}
	if(!req.body.message) {
		res.status(400).send("Bad Request: missing message key");
		return;
	}

	chats.findOne({_id: new objectID(req.body.chatID), "members.user": req.user._id}, function(err, chat) {
		if(err) {
			console.log("chat.sendMessage database err:");
			console.log(err);
			res.status(500).send("Database error occured!");
		} else if(!chat) {
			console.log("User: '" + req.user.email + "' attempted to access chat: '" + req.body.chatID + "':");
			res.status(403).send("Chat not found or you are not a member of the chat");
		} else {
			chats.updateOne({_id: chat._id}, {$push: {messages: {message: req.body.message, date: new Date().toString(), sender: req.user._id}}}, function(err, result) {
				if(err) {
					res.status(500).send("Database error occurred!");
				} else {
					firebase.notifyDevices(chat._id, req.user);
					res.send("Message sent");
				}
			});
		}
	});
};

/*	Function for getting the number of messages to a specific DM	*/
module.exports.getMessageCount = function(req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	chats.findOne({_id: new objectID(req.body.chatID), "members.user": req.user._id}, function(err, chat) {
		if(err) {
			console.log("chat.getMessageCount database err:");
			console.log(err);
			res.status(500).send("Database error occured!");
		} else if(!chat) {
			console.log("User: '" + req.user.email + "' attempted to access chat: '" + req.body.chatID + "':");
			console.log(chat);
			res.status(403).send("Chat not found or you are not a member of the chat");
		} else {
			res.json({chatID: req.body.chatID, m_count: chat.messages.length});
		}
	});

};

/*	Function to retrieve messages between two users	*/
module.exports.getMessages = function (req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}

	chats.findOne({_id: new objectID(req.body.chatID), "members.user": req.user._id}, function(err, result) {
		if(err) {
			console.log("chat.getMessages database error");
			console.log(err);
			res.status(500).send("Database error occured!");
		} else if(!result) {
			console.log("User: '" + req.user.email + "' attempted to access chat: '" + req.body.chatID + "':");
			console.log(result);
			//console.log(chat);	chat never defined
			res.status(403).send("Chat doesn't exist or you are not a member of the chat");
		} else {
			console.log("Getting message. Request body:", req.body);
			if(!req.body.start) {
				res.json({messages:result.messages})
			}else if(req.body.end) {
				res.json({messages: result.messages.slice(Math.max(req.body.start, 0), Math.min(req.body.end, result.messages.length))});
			} else  {
				console.log("Getting Message from", req.body.start);
				res.json({messages: result.messages.slice(Math.max(req.body.start, 0))});
			}
		}
	});
};

/*	Function which starts a DM between two users	*/
module.exports.startDM = function(req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	//console.log('req.body: ' + req.body);
	console.log('user id:' + req.user._id + ' is trying to message user id: ' + req.body.dm_user)
	if(!req.body.dm_user) {
		console.log("Missing dm_user in request");
		res.status(400).send("Bad Request: missing dm_user key");
		return;
	}
	users.findOne({_id: new objectID(req.body.dm_user)}, function(err, user) {
		if(err) {
			console.log("chat.startDM database error");
			res.status(500).send("Database error occured!");
			return;
		}
		if (!user) {
			console.log("Err: could not find user");
			res.send("Could not find user");
			return;
		}
		chats.findOne({$and: [{isDM: true, "members.user": user._id}, {isDM: true, "members.user": req.user._id}]}, function(err, chat) {
			if(err) {
				console.log("chat.startDM database error");
				res.status(500).send("Database error occured!");
				return;
			}
			if(chat) {
				console.log("Error: Tried to create exisiting dm");
				res.json({_id:chat._id}) //should just return the chat _id if chat already exists
				//res.send("Chat exists");
				return;
			}
			chats.insertOne({isDM: true, members: [{user: req.user._id, muted: false}, {user: user._id, muted: false}]}, function(err, result) {
				if(err) {
					console.log("chat.startDM database error");
					res.status(500).send("Database error occurred!");
					return;
				}
				users.updateMany({$or: [{_id: req.user._id}, {_id: user._id}]}, {$push: {dms: result.insertedId}}, function(err, u_result) {
					if(err) {
						console.log("chat.startDM database error");
						res.status(500).send("Database error occurred!");
						return;
					}
					if(u_result.modifiedCount != 2) {
						console.log("chat.startDM did not insert two items");
						res.status(500).send("Database error occurred!");
						return;
					}
					console.log("DM successfully created");

					chats.findOne({$and: [{isDM: true, "members.user": user._id}, {isDM: true, "members.user": req.user._id}]}, function(err, chat) {
						res.json({_id:chat._id}) //maybe works
					});

					//res.json({_id:result._id})
				});

			});
		});
	});
};

/*	Function to start a class message board	*/
module.exports.startClassDM = function(req, res) {		//just copy-pasted above and changed it to class for message board
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	if(!classes) {classes = require('./index.js').classes;}
	//console.log('req.body: ' + req.body);
	console.log('user id:' + req.user._id + ' is trying to message class: ' + req.body.classID)
	if(!req.body.classID) {
		console.log("Missing classID in request");
		res.status(400).send("Bad Request: missing classID key");
		return;
	}
	classes.findOne({name: req.body.classID}, function(err, className) {
		if(err) {
			console.log("chat.startClassDM database error");
			res.status(500).send("Database error occured!");
			return;
		}
		if (!className) {
			console.log("Err: could not find class");
			res.send("Could not find class");
			return;
		}
		chats.findOne({isDM: true, "className": req.body.classID}, function(err, chat) {
			if(err) {
				console.log("chat.startDM database error");
				res.status(500).send("Database error occured!");
				return;
			}
			if(chat) {
				console.log("Error: Tried to create exisiting ClassDM, sending chatID as res");
				res.json({_id:chat._id}) //should just return the chat _id if chat already exists
				//res.send("Chat exists");
				return;
			}
			chats.insertOne({isDM: true, "className": req.body.classID}, function(err, result) {
				if(err) {
					console.log("chat.startDM database error");
					res.status(500).send("Database error occurred!");
					return;
				}
				/*users.updateMany({$or: [{_id: req.user._id}, {_id: user._id}]}, {$push: {dms: result.insertedId}}, function(err, u_result) {
					if(err) {
						console.log("chat.startDM database error");
						res.status(500).send("Database error occurred!");
						return;
					}
					if(u_result.modifiedCount != 2) {
						console.log("chat.startDM did not insert two items");
						res.status(500).send("Database error occurred!");
						return;
					}


					//res.json({_id:result._id})
				});*/

				/*chats.findOne({$and: [{isDM: true, "members.user": user._id}, {isDM: true, "members.user": req.user._id}]}, function(err, chat) {
					res.json({_id:chat._id}) //maybe works
				});*/
			});
			console.log("ClassDM successfully created, sending chatID as res");
			chats.findOne({"className": req.body.classID}, function(err, ret) {
				res.json({_id:ret._id}) //maybe works
			});
		});
	});
};

/*	Function to send a message to a class message board	*/
module.exports.sendClassMessage = function (req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	if(!classes) {classes = require('./index.js').classes;}

	if(!req.body.chatID) {
		res.status(400).send("Bad Request: missing chatID key");
		return;
	}
	if(!req.body.message) {
		res.status(400).send("Bad Request: missing message key");
		return;
	}

	chats.findOne({_id: new objectID(req.body.chatID), "className": req.body.classID}, function(err, chat) {
		if(err) {
			console.log("chat.sendMessage database err:");
			console.log(err);
			res.status(500).send("Database error occured!");
		} else if(!chat) {
			console.log("User: '" + req.user.email + "' attempted to message class chat: '" + req.body.chatID + "':");
			res.status(403).send("Chat not found or you are not a member of the chat");
		} else {
			chats.updateOne({_id: chat._id}, {$push: {messages: {message: req.body.message, date: new Date().toString(), sender: req.user._id}}}, function(err, result) {
				if(err) {
					res.status(500).send("Database error occurred!");
				} else {
					firebase.notifyDevices(chat._id);
					res.send("Message sent");
				}
			});
		}
	});
};
