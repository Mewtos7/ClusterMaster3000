# ClusterManager3000

## Introduction
This project should provide an application in c# to create, manage and scale an lxd cluster as well as virtual machines running on created cluster.
It should automatically detect high load on affected lxd cluster members (hosts) and add more servers to the lxd cluster and move virtual machines and containers ( with no or minimal constraints.

## Features implemented
- Database
	- SQLite Databases to store informations about lxd cluster members and hosted machines/containers
- Platform
	- Hetzner with API Key