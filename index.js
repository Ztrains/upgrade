const fs = require('fs'); // required to read https certs
const https = require('https');
const express = require('express');
const mongodb = require('mongodb');
const bodyParser = require('body-parser');
const app = express();

app.use(bodyParser.json());


//const port = 3000
var db;
var localTestUrl = 'mongodb://localhost:27017/test'

mongodb.MongoClient.connect(process.env.MONGODB_URI || localTestUrl, function (err, database) {
  if (err) {
    console.log('ERROR:', err);
    process.exit(1);
  }

  // Save database object from the callback for reuse.
  db = database;
  console.log("Database connection ready");

  // Initialize the app.
  var server = app.listen(process.env.PORT || 8080, function () {
    var port = server.address().port;
    console.log("App now running on port", port);
  });
});

app.get('/',(request,response)=>{
    response.send('Welcome to upgrade')
});

//setup https credentials
var privKey = fs.readFileSync('ourprivKey.key', 'utf8');
var certificate = fs.readFileSync('ourcert.crt', 'utf8');
var options = {key: privKey, cert: certificate};

//setup a https server server to listen on port 3000
var httpsServer = https.createServer(options, app);
httpsServer.listen(3000);

//probably shouldn't setup a unsecure conenction to start
/*app.listen(port, (err)=>{
    if (err) return console.log('error', err)
    console.log('server is listening on ' + port)
})*/
