const objectID = require('mongodb').ObjectID
var users;
var chats;

module.exports.getDms= function(req, res) {
	res.json({dms: req.user.dms});
}

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
			console.log(chat);
			res.status(403).send("Chat not found or you are not a member of the chat");
		} else {
			chats.updateOne({_id: chat._id}, {$push: {messages: {message: req.body.message, date: new Date(), sender: req.user._id}}});
			res.send("Message sent");
		}
	});
};

module.exports.getMessageCount = function(req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	chats.findOne({_id: req.body.chatID, members: req.user._id}, function(err, chat) {
		if(err) {
			console.log("chat.getMessageCount database err:");
			console.log(err);
			res.status(500).send("Database error occured!");
		} else if(!chat) {
			console.log("User: '" + req.user.email + "' attempted to access chat: '" + req.body.chatID + "':");
			console.log(chat);
			res.status(403).send("Chat not found or you are not a member of the chat");
		} else {
			res.json({chatID: req.body.chatID, m_count: chat.m_count});
		}
	});

};

module.exports.getMessages = function (req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	if(!req.body.chatID) {
		res.status(400).send("Bad Request: missing chatID key");
		return;
	}
	if(!req.body.num) {
		res.status(400).send("Bad Request: missing num key");
		return;

	}

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
			res.json({messages:result.messages})
			//res.json({messages: result.messages.slice(Math.max(result.messages.length - req.body.num, 1))});
		}
	});

};

module.exports.startDM = function(req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	console.log(req.body);
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
		chats.findOne({isDM: true, "members.user": user._id}, function(err, chat) {
			if(err) {
				console.log("chat.startDM database error");
				res.status(500).send("Database error occured!");
				return;
			}
			if(chat) {
				console.log("Error: Tried to create exisiting dm");
				res.send("Chat exists");
				return;
			}
			chats.insertOne({isDM: true, members: [{user: req.user._id, muted: false}, {user: user._id, muted: false}], messageCount: 0}, function(err, result) {
				if(err) {
					console.log("chat.startDM database error");
					res.status(500).send("Database error occurred!");
					return;
				}
				users.updateMany({$or: [{_id: req.user._id}, {_id: user._id}]}, {$push: {chats: result.insertedId}}, function(err, u_result) {
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
					res.send("DM successfully created");
				});

			});
		});
	});
};
/******************************************
 * left over from previous file
 * ****************************************
var io = require('socket.io')
var socket = io()	//change to process.env.PORT when in prod
var usernames = {};

function check_if_exists(id) {
	for (var name in usernames) {
		if (name === id) {
			return true;
		}
	}
	return false;
}

socket.on("connection", (client)=>{
	console.log('user connected')

	client.on('adduser', (username)=>{
		//store username in socket session for this client
		client.username = username;
		//add clients username to global list
		if (check_if_exists(username) === false)
			usernames[username] = client.id;
	});

	// when the user sends a private message to a user.. perform this
	client.on('msg_user', (user_to, user_from, msg)=>{
		console.log("From user: "+user_from);
		console.log("To user: "+user_to);
		//console.log(usernames);
		io.sockets.client(usernames[user_to]).emit('updatechat', msg);

	});
});
*/
