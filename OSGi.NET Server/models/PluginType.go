package models

import ()

type PluginType struct {
	PluginTypeId   string `bson:"plugintypeid"`
	PluginTypeName string `bson:"plugintypename"`
}
