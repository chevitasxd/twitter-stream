const express = require('express');
const app = express();
const port = 3000;

/*
   Here, for route "/", use "static" folder.
   In "static" folder you can put now html, js, css files.
 */
app.use('/', express.static('public'));

app.listen(port, () => console.log(`Example app listening on port ${port}!`));