package main

import (
	"log"
	"os"
	"os/signal"

	"nathan.dev/consent/internal/server"
)

func main() {
	log.Println("Starting web service")
	service, err := server.StartService()
	if err != nil {
		log.Fatalln("service init fail", err)
	}

	ch := make(chan os.Signal, 1)
	signal.Notify(ch, os.Interrupt)
	sig := <-ch
	service.Close()
	log.Fatalln("Shutdown, signal:", sig)
}
