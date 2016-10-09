package pluginTypeService

import (
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"OSGiServer/models"
	"OSGiServer/services"
)

func init() {
}

func AddPluginType(service *services.Service, pluginType *models.PluginType) (string, error) {

	tracelog.Startedf(service.UserID, "AddPluginType", "pluginType%+v", &pluginType)

	dberr := service.DBActionOSGi("PluginType",
		func(collection *mgo.Collection) error {
			_, err1 := collection.Upsert(bson.M{"plugintypeid": pluginType.PluginTypeId}, &pluginType)
			return err1
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "AddPluginType")
		return "", dberr
	}

	tracelog.Completed(service.UserID, "AddPluginType")

	return pluginType.PluginTypeId, dberr
}

func GetPluginType(service *services.Service, plugintypeid string) (*models.PluginType, error) {

	tracelog.Startedf(service.UserID, "GetPluginType", "plugintypeid[%s]", plugintypeid)

	result := models.PluginType{}
	dberr := service.DBActionOSGi("PluginType",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"plugintypeid": plugintypeid}).One(&result)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetPluginType")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetPluginType", "pluginType%+v", &result)

	return &result, dberr
}

func GetAllPluginTypes(service *services.Service) ([]models.PluginType, error) {

	tracelog.Started(service.UserID, "GetAllPluginTypes")

	var results []models.PluginType
	dberr := service.DBActionOSGi("PluginType",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{}).All(&results)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetAllPluginTypes")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetAllPluginTypes", "pluginType%+v", &results)

	return results, dberr
}

func DeletePluginType(service *services.Service, plugintypeid string) error {
	tracelog.Started(service.UserID, "DeletePluginType")

	dberr := service.DBActionOSGi("PluginType",
		func(collection *mgo.Collection) error {
			return collection.Remove(bson.M{"plugintypeid": plugintypeid})
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "DeletePluginType")
		return dberr
	}

	tracelog.Completed(service.UserID, "DeletePluginType")

	return nil
}
