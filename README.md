# ClusterMaster3000

## Introduction
This project should provide an application in c# to create, manage and scale an lxd cluster as well as virtual machines running on created cluster.
It should automatically detect high load on affected lxd cluster members (hosts) and add more servers to the lxd cluster and move virtual machines and containers ( with no or minimal constraints.

## Features implemented
- Platform
	- Create/Delete Servers with Hetzner API
	- Create SSHKeys with Hetzner API
	- Auto-Generate SSH Keys, encrypt wthem and save in database
- Database
	- SQLite Database
	- Save server information & ssh keyss in SQLite database table
- App configuration
	- Configuration file for app settings

## Roadmap
- Next
	- Add Unit Tests
	- Add IHttpClientFactory
	- Cloud-init: Upgrade server at start, install lxd and create lxd cluster
	- Add cluster member to existent lxd cluster when creating new server
	- Add Logging and proper exception handling strategy

## Contribution
Just ask me, I would be happy for every help.

