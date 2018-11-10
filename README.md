# ruven
Game for the Github Game Off 2018 - Themed HYBRID!

# Running the server
* `npm install`
* development: `npm run dev`
* production: `npm start`

A web monitor client is available at `<host and port>/colyseus` (by default `localhost:3000/colyseus`)

# Debugging the server
* `npm run dev:debug`
* open chrome at `about:inspect`
* open choose the server from the list, and enjoy debugging :)

To enable colyseus debug prints, add `colyseus:*` to the `DEBUG` environment parameter in
The relevant npm script in `package.json` (f.e: `DEBUG=ruven*` -> `DEBUG=ruven*,colyseus:*`)

# Client Side
* Unity 2019.1a
* [Colyseus Unity 3D](https://github.com/gamestdio/colyseus-unity3d)

# Server Side
* Node.js
* Typescript
* [Colyseus](https://github.com/gamestdio/colyseus/)
