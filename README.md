# e-Learning Platform

---

## Requirements

- Ensure that [Node.js](https://nodejs.org/en/) is installed

## Running

```shell
npm run start:dev
```

## Testing

```shell
npm run test
```

or to run in "Watch" mode where Jest stays active and waits for changes to source code...

```shell
npm run test:watch
```

## MongoDB Config.

In order to switch from using the live mLab cluster to a local dev server for usage on eduroam, change line 21 in `server/server.js` to...

`mongoose.connect(config.db_dev);`

From there, create a directory in the root called `data` and initialise a MongoDB instance in that directory, for example on Windows:

`"C:\Program Files\MongoDB\Server\3.6\bin\mongod.exe" --dbpath="{ABSOLUTE-PATH-TO-ROOT}/data"`

You will have to export any new data from mLab and import it into your local instance.

Alternatively, keep `mongoose.connect(config.db);` and interact directly with the mLab cluster.