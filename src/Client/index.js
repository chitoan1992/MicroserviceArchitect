var express = require("express");
var app = express();
var path = require("path");
var request = require("request");
// var Keycloak = require('keycloak-connect');
const router = express.Router()

// var keycloak = new Keycloak();

app.use(express.static(__dirname + '/assets'));
// app.use( keycloak.middleware() );

const bodyParser = require('body-parser');
app.use(bodyParser.urlencoded({
    extended: true
}));

app.post('/', function (req, res) {
    var options = {
        method: 'POST',
        url: 'http://192.168.1.13:8080/auth/realms/sample/protocol/openid-connect/token',
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        form: {
            client_id: 'sample-api',
            client_secret: '3caddc71-ee0a-4bb3-a776-6507a5e8292a',
            grant_type: 'password',
            username: 'test',
            password: 'test'
        }
    };

    request(options, function (error, response, body) {
        if (error) throw new Error(error);
        var options = {
            method: 'GET',
            url: 'http://192.168.1.13:8000/chuck',
            headers: {
                'Cache-Control': 'no-cache',
                Authorization: "Bearer " + JSON.parse(body).access_token,
            }
        };
        request(options, function (error, response, body) {
            if (error) throw new Error(error);
            console.log(options.headers.Authorization);
            console.log(body);
        });
    });
});

app.get('/', function (req, res) {
    res.sendFile(path.join(__dirname + '/pages-login.html'));
});

app.listen(3000);

console.log("Running at Port 3000");
