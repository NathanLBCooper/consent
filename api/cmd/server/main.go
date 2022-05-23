package main

import (
	"log"
	"os"
	"os/signal"

	"nathan.dev/consent/internal"
	"nathan.dev/consent/internal/httpserver"
)

func main() {
	log.Println("Hello world")
	// db, err := mongo.New("mongodb://localhost:27015", "consent")
	// if err != nil {
	// 	log.Fatalln("mongo init fail", err)
	// }
	api, err := internal.NewAPI(nil, nil)
	if err != nil {
		log.Fatalln("api init fail", err)
	}
	app, err := httpserver.New(api, ":9999")
	if err != nil {
		log.Fatalln("httpserver init fail", err)
	}
	ch := make(chan os.Signal, 1)
	signal.Notify(ch, os.Interrupt)
	sig := <-ch
	app.Close()
	log.Fatalln("Shutdown, signal:", sig)
}
