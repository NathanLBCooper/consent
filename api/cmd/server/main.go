package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"os/signal"

	"gopkg.in/yaml.v2"

	"nathan.dev/consent/internal/server"
)

func main() {
	config, err := readConfig("config.yaml")
	if err != nil {
		log.Fatal("config read fail", err)
	}

	overwriteConfigFromEnv(config)

	log.Println("Starting web service")
	service, err := server.StartService(config)
	if err != nil {
		log.Fatalln("service init fail", err)
	}

	ch := make(chan os.Signal, 1)
	signal.Notify(ch, os.Interrupt)
	sig := <-ch
	service.Close()
	log.Fatalln("Shutdown, signal:", sig)
}

func readConfig(filename string) (*server.Config, error) {
	buf, err := ioutil.ReadFile(filename)
	if err != nil {
		return nil, err
	}

	c := &server.Config{}
	err = yaml.Unmarshal(buf, c)
	if err != nil {
		return nil, fmt.Errorf("in file %q: %v", filename, err)
	}

	return c, nil
}

func overwriteConfigFromEnv(config *server.Config) {
	// todo, there's probably a magic way. Just manually do mongo uri for now
	uri := os.Getenv("mongoconnectionsettings__uri")
	if uri != "" {
		config.MongoConnectionSettings.Uri = uri
	}
}
