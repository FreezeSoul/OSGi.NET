package models

import (
	"time"
)

type Project struct {
	ProjectId     string    `bson:"projectid"`
	CreateTime    time.Time `bson:"createtime"` //创建时间
	ProjectName   string    `bson:"projectname"`
	ProjectIntro  string    `bson:"projectintro"`
	ProjectDetail string    `bson:"projectdetail"`
}

type ProjectPluginMap struct {
	ProjectId      string `bson:"projectid"`
	PluginId       string `bson:"pluginid"`
	PluginManifest string `bson:"pluginmanifest"` //插件清单
}
