package models

import (
	"time"
)

type Plugin struct {
	PluginId        string    `bson:"pluginid"`        //插件ID
	CreateTime      time.Time `bson:"createtime"`      //创建时间
	UpdateTime      time.Time `bson:"updatetime"`      //更新时间
	PluginName      string    `bson:"pluginname"`      //插件名称
	PluginImage     string    `bson:"pluginimage"`     //插件图片
	PluginIntro     string    `bson:"pluginintro"`     //插件简介
	PluginAuthor    string    `bson:"pluginauthor"`    //插件作者
	PluginDetail    string    `bson:"plugindetail"`    //插件说明
	PluginTypeId    string    `bson:"plugintypeid"`    //插件类型ID
	PluginIsShare   bool      `bson:"pluginisshare"`   //是否共享插件
	PluginDocument  string    `bson:"plugindocument"`  //插件手册
	DependPluginIds []string  `bson:"dependpluginids"` //依赖插件ID
}

type PluginVersion struct {
	PluginId        string    `bson:"pluginid"`        //插件ID
	Version         string    `bson:"version"`         //版本
	TraceInfo       string    `bson:"traceinfo"`       //版本说明
	CreateTime      time.Time `bson:"createtime"`      //创建时间
	PluginPackage   string    `bson:"pluginpackage"`   //插件模块
	PluginInterface string    `bson:"plugininterface"` //插件接口
}
