var users;
var chats;

module.exports.getDms= function(req, res) {
	res.json({dms: req.user.dms});
}

module.exports.sendMessage = function (req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
//	chats.findOne({req
		
	
});

module.exports.getMessageCount = function(req, res) {
	if(!chats) {chats = require('./index.js').chats;}
	if(!users) {users = require('./index.js').users;}
	chats.findOne({_id: req.body.chatID, members: req.user._id}, function(err, chat) {
		if(err) {
			console.log("chat.getMessageCount database err:");
			console.log(err);
			res.send("Database error occured!");
		} else if(!chat) {
			console.log("User: '" + req.user.email + "' attempted to access chat: '" + req.body.chatID + "':");
			console.log(chat);
			res.status(403).send("Chat not found or you are not a member of the chat");	
		} else {
			
		}
	});

});

module.exports.getMessages = function (req, res) {

});
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
