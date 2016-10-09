package models

import (
	"time"
)

type Client struct {
	ClientMac   string    `bson:"clientmac"`
	ClientIp    string    `bson:"clientip"`
	SystemName  string    `bson:"systemname"`
	MonitorPort string    `bson:"monitorport"`
	SystemInfo  string    `bson:"systeminfo"`
	LaunchTime  time.Time `bson:"launchtime"`
	Status      string    `bson:"status"`
}
