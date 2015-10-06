# node-red-contrib-virual-app-node
A collection of [Node-RED](http://nodered.org) nodes to communicate Node-Red node and apps.

## Install
Use npm to command to install this package locally in the Node-RED modules directory
```bash
npm install node-red-contrib-virual-app-node 
```
or install in it globally with the command
```bash
npm install node-red-contrib-virual-app-node -g 
```

## Nodes included in the package
**Export** Export data to apps.

**Import** Import data to node from apps.

**Delegate** Export data to apps and import data to node.

## Usage example
![Flow](./node-red-contrib-virual-app-node_example.png)
Simple usage of the plugin in Node-RED, a message with ON or 1 will turn on the node, otherwise a message with OFF or 0 will turn off the node.
```json
[{"id":"a43c149e.5bc3e8","type":"VAN-EXPORT","name":"","topic":"VAN Export","token":"","x":351,"y":104,"z":"92616130.6d9ea","wires":[]},{"id":"220d8ba1.ddf274","type":"inject","name":"","topic":"","payload":"","payloadType":"date","repeat":"","crontab":"","once":false,"x":182,"y":104,"z":"92616130.6d9ea","wires":[["a43c149e.5bc3e8"]]},{"id":"5cc5e0aa.a33a2","type":"debug","name":"","active":true,"console":"false","complete":"false","x":355,"y":185,"z":"92616130.6d9ea","wires":[]},{"id":"c404147d.3bfbe8","type":"VAN-IMPORT","name":"","topic":"VAN Import","token":"","x":182,"y":185,"z":"92616130.6d9ea","wires":[["5cc5e0aa.a33a2"]]},{"id":"6b3d3601.94c2c8","type":"VAN-DELEGATE","name":"","topic":"VAN Delegate","token":"","x":364,"y":259,"z":"92616130.6d9ea","wires":[["4e8a2d2e.b175d4"]]},{"id":"f823c1f8.07dc4","type":"inject","name":"","topic":"","payload":"","payloadType":"date","repeat":"","crontab":"","once":false,"x":188,"y":259,"z":"92616130.6d9ea","wires":[["6b3d3601.94c2c8"]]},{"id":"4e8a2d2e.b175d4","type":"debug","name":"","active":true,"console":"false","complete":"false","x":546,"y":259,"z":"92616130.6d9ea","wires":[]}]

## Tested devices 

## History
- 0.0.1 - October 2015 : Initial Release

## Authors
* Neo Lo (https://github.com/neo7206)

## License
Copyright 2014, 2015 ADVANTECH Corp. under [the Apache 2.0 license](LICENSE).