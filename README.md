# ClusterMaster3000

## Introduction
This project should provide an application in c# to create, manage and scale an lxd cluster as well as virtual machines running on created cluster.
It should automatically detect high load on affected lxd cluster members (hosts) and add more servers to the lxd cluster and move virtual machines and containers ( with no or minimal constraints.

## Features implemented
- Platform
	- Create/Delete Servers with Hetzner API
	- Create SSHKeys with Hetzner API
- Database
	- SQLite Database (Not functional at this point)
- App configuration
	- Configuration file for app settings

## Roadmap
- Next
	- Save server informations in SQLite database table
	- Auto-Generate SSH Keys and save them in a secure place
	- Cloud-init: Upgrade server at start, install lxd and create lxd cluster
- Mid-target
	- Add cluster member to existent lxd cluster
- Future
	- Nothing in mind :)


