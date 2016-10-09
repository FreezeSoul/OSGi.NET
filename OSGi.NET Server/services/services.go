package services

import (
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"

	"OSGiServer/utils/helper"
	"OSGiServer/utils/mongo"
)

//** TYPES

type (
	// Service contains common properties for all services.
	Service struct {
		MongoSession *mgo.Session
		UserID       string
	}
)

//** PUBLIC FUNCTIONS

// Prepare is called before any controller.
func (service *Service) Prepare() (err error) {
	service.MongoSession, err = mongo.CopyMonotonicSession(service.UserID)
	if err != nil {
		tracelog.Error(err, service.UserID, "Service.Prepare")
		return err
	}

	return err
}

// Finish is called after the controller.
func (service *Service) Finish() (err error) {
	defer helper.CatchPanic(&err, service.UserID, "Service.Finish")

	if service.MongoSession != nil {
		mongo.CloseSession(service.UserID, service.MongoSession)
		service.MongoSession = nil
	}

	return err
}

// DBAction executes the MongoDB literal function
func (service *Service) DBAction(databaseName string, collectionName string, dbCall mongo.DBCall) (err error) {
	return mongo.Execute(service.UserID, service.MongoSession, databaseName, collectionName, dbCall)
}

func (service *Service) DBActionOSGi(collectionName string, dbCall mongo.DBCall) (err error) {
	return mongo.ExecuteDb(service.UserID, service.MongoSession, collectionName, dbCall)
}
