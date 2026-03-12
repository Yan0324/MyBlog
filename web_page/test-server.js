const { createServer } = require('http');
const { readFileSync } = require('fs');
const { join } = require('path');

const server = createServer((req, res) => {
  console.log(`Request: ${req.url}`);
  
  if (req.url === '/' || req.url === '/index.html') {
    try {
      const content = readFileSync(join(__dirname, 'public/index.html'), 'utf8');
      res.writeHead(200, { 'Content-Type': 'text/html' });
      res.end(content);
    } catch (err) {
      res.writeHead(500);
      res.end('Error loading index.html');
    }
  } else if (req.url === '/Animation.lottie') {
    try {
      const content = readFileSync(join(__dirname, 'public/Animation.lottie'));
      res.writeHead(200, { 'Content-Type': 'application/octet-stream' });
      res.end(content);
    } catch (err) {
      res.writeHead(404);
      res.end('Animation file not found');
    }
  } else {
    res.writeHead(404);
    res.end('Not found');
  }
});

const PORT = 8080;
server.listen(PORT, () => {
  console.log(`Test server running on http://localhost:${PORT}`);
  console.log('You can test the Animation.lottie file at: http://localhost:${PORT}/Animation.lottie');
});