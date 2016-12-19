# prototypes
Disposable prototypes for our experiments

## Setup
* Install Docker;
* Setup a `/work` shared folded on VirtualBox VM poiting to the directory containing this project


## Build

On Windows run:

`build.bat`

On OS X and Linux run:

`./build.sh`


## Run

On Windows run:

`run.bat`

On OS X and Linux run:

`./run.sh`


## Usage

Use the API, visiting:

`[GET] /api/cardholders/counter` to count the persisted card holders

`[PUT] api/cardholder/{id}` to create a card holder
`[GET] api/cardholder/{id}` to read a card holder


The `[GET]` call `api/cardholder/1` is pre-populated and should return:

```xml
<CardHolder>
  <ID>1</ID>
  <Firstname>Marco</Firstname>
  <Lastname>Bernasconi</Lastname>
</CardHolder>
```
