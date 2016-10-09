package clientService

import (
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"OSGiServer/models"
	"OSGiServer/services"
)

func init() {
}

func AddClient(service *services.Service, client *models.Client) (string, error) {

	tracelog.Startedf(service.UserID, "AddClient", "client%+v", &client)

	dberr := service.DBActionOSGi("Client",
		func(collection *mgo.Collection) error {
			_, err1 := collection.Upsert(bson.M{"clientmac": client.ClientMac}, &client)
			return err1
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "AddClient")
		return "", dberr
	}

	tracelog.Completed(service.UserID, "AddClient")

	return client.ClientMac, dberr
}

func GetClient(service *services.Service, clientMac string) (*models.Client, error) {

	tracelog.Startedf(service.UserID, "GetClient", "clientMac[%s]", clientMac)

	result := models.Client{}
	dberr := service.DBActionOSGi("Client",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"clientmac": clientMac}).One(&result)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetClient")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetClient", "client%+v", &result)

	return &result, dberr
}

func GetAllClients(service *services.Service) ([]models.Client, error) {

	tracelog.Started(service.UserID, "GetAllClients")

	var results []models.Client
	dberr := service.DBActionOSGi("Client",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{}).Sort("systemname").All(&results)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetAllClients")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetAllClients", "clients%+v", &results)

	return results, dberr
}

func UpdateStatus(service *services.Service, clientMac string, status string) error {

	tracelog.Startedf(service.UserID, "UpdateStatus", "clientMac[%s],status[%s]", clientMac, status)

	dberr := service.DBActionOSGi("Client",
		func(collection *mgo.Collection) error {
			change := bson.M{"$set": bson.M{"status": status}}
			return collection.Update(bson.M{"clientmac": clientMac}, change)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "UpdateStatus")
		return dberr
	}

	tracelog.Completed(service.UserID, "UpdateStatus")
	return nil
}
