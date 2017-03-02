const fs = require('fs'); // required to read https certs
const https = require('https');
const express = require('express');
const mongodb = require('mongodb');
const bodyParser = require('body-parser');
const stormpath = require('express-stormpath');
var app = express();

app.use(stormpath.init(app, {
  expand: {
    customData: true,
  },
  web: {
    login: {
      enabled: true,
      nextUri: "/dashboard"
    }
  }
}))

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended: false}))

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

app.get('/',stormpath.loginRequired, (req,res)=>{
    res.send('Welcome to upgrade, ' + req.user.givenName)
});

app.get('/classList', stormpath.loginRequired, (req,res)=> {
    res.type('json');
    var list;
    db.collection('classes', (err, collection)=>{
        if (err) {
          console.log('ERROR:', err);
          process.exit(1);
        }
        else {
            list = collection.find({},{name:1, _id:0}).toArray();
        }
    })
    res.json(list);

    //res.json(db.classes.find({},{name:1, _id:0}).toArray());
})
