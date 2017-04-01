var io = require('socket.io')(app)
var usernames = {};

function check_if_exists(id) {
	for (var name in usernames) {
		if (name === id) {
			return true;
		}
	}
	return false;
}

io.on("connection", (socket)=>{
	console.log('user connected')

	socket.on('adduser', (username)=>{
		//store username in socket session for this client
		socket.username = username;
		//add clients username to global list
		if (check_if_exists(username) === false)
			usernames[username] = socket.id;
	});

	// when the user sends a private message to a user.. perform this
	socket.on('msg_user', function(user_to, user_from, msg) {
		console.log("From user: "+user_from);
		console.log("To user: "+user_to);
		//console.log(usernames);
		io.sockets.socket(usernames[user_to]).emit('updatechat', msg);

	});
});
