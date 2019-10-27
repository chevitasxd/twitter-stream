const express = require('express');
var cors = require('cors');
const app = express();
const port = 3000;

/*
   Here, for route "/", use "static" folder.
   In "static" folder you can put now html, js, css files.
 */
app.use(cors());
app.get('/tweethuburl', (req, res) => res.send(process.env.TWEET_HUB_URL + '/tweethub'));
app.use('/', express.static('public'));
app.listen(port, () => console.log(`Example app listening on port ${port}!`));