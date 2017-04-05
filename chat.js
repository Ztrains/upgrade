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
