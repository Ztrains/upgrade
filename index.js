const fs = require('fs'); // required to read https certs
const https = require('https');
const express = require('express');
const mongodb = require('mongodb');
const bodyParser = require('body-parser');
const stormpath = require('express-stormpath');
const app = express();

app.use(bodyParser.json());
app.use(stormpath.init(app, {
  expand: {
    customData: true,
  },
  web: {
    produces: ['application/json']
  }
}))


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
  app.on('stormpath.ready', function() {
    var server = app.listen(process.env.PORT || `8080`, function () {
        var port = server.address().port;
        console.log("App now running on port", port);
    });
  });
});

app.get('/',(request,response)=>{
    response.send('Welcome to upgrade')
});

app.get('/login', stormpath.apiAuthenticationRequired, (req,res)=>{
    res.json({username: req.user.customData.username || "This is stormpath"})
})

//setup https credentials
var privKey = fs.readFileSync('ourprivKey.key', 'utf8');
var certificate = fs.readFileSync('ourcert.crt', 'utf8');
var options = {key: privKey, cert: certificate};

//setup a https server server to listen on port 3000
//var httpsServer = https.createServer(options, app);
//httpsServer.listen(3000);

//probably shouldn't setup a unsecure conenction to start
/*app.listen(port, (err)=>{
    if (err) return console.log('error', err)
    console.log('server is listening on ' + port)
})*/
