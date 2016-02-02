/**
 * Copyright 2014, 2015 ADVANTECH Corp.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 **/

// Dependency - dht sensor package
var zmq = require('zmq');
var socketpub = zmq.socket('pub');
var socketrep = zmq.socket('rep');

function sleep(milliseconds) {
	var start = new Date().getTime();
	for (var i = 0; i < 1e7; i++) {
		if ((new Date().getTime() - start) > milliseconds){
			break;
		}
	}
}

module.exports = function(RED) {
   "use strict";
	
	process.setMaxListeners(0);
	
	socketpub.bind('tcp://127.0.0.1:22220', function(err) {
	if (err) throw err;
	console.log('Socketpub bound!');
	});
	
	socketrep.bind('tcp://127.0.0.1:22222', function(err) {
	if (err) throw err;
	console.log('Socketrep bound!');
	});
	
	// The main node definition - most things happen in here
	function VAN(config) {
		// Create a RED node
		RED.nodes.createNode(this, config);

		// Store local copies of the node configuration (as defined in the .html)
		var node = this;
		
		this.topic = config.topic;

		socketrep.on('message', function(data, msg) {
			//console.log('ZMQ server received: ' + data.toString());
			socketrep.send('Server received success');
			var msgarray = data.toString().split(",");
			var msg = {};
			if(msg && msgarray[0] == config.token)
			{
				msg.payload = msgarray[1];
				for (var i=2; i<msgarray.length; i++) {
					msg.payload = msg.payload + ',' + msgarray[i];
				}
				node.send(msg);
			}
			sleep(10)
			});

		// Read the data & return a message object
		this.read = function(msgIn) {
			var msg = msgIn ? msgIn : {};
			
			if (!(typeof config.token !== 'undefined' && config.token !== ''))
				socketpub.send(msg.payload);
			else
				socketpub.send([config.token, msg.payload]);
		};

		// respond to inputs....
		this.on('input', function (msg) {
			this.read(msg);
		});

	}

	// Register the node by name.
	RED.nodes.registerType("VAN-EXPORT", VAN);
	RED.nodes.registerType("VAN-IMPORT", VAN);
	RED.nodes.registerType("VAN-DELEGATE", VAN);
}
