// Package docs GENERATED BY SWAG; DO NOT EDIT
// This file was generated by swaggo/swag
package docs

import "github.com/swaggo/swag"

const docTemplate = `{
    "schemes": {{ marshal .Schemes }},
    "swagger": "2.0",
    "info": {
        "description": "{{escape .Description}}",
        "title": "{{.Title}}",
        "contact": {},
        "version": "{{.Version}}"
    },
    "host": "{{.Host}}",
    "basePath": "{{.BasePath}}",
    "paths": {
        "/v1/account/{id}": {
            "get": {
                "summary": "Get an account",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "Account Id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/user.AccountModel"
                        }
                    }
                }
            }
        },
        "/v1/health": {
            "get": {
                "description": "Get component by component health status",
                "summary": "Get Health",
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/controllers.HealthResponse"
                        }
                    }
                }
            }
        },
        "/v1/organization/{id}": {
            "get": {
                "summary": "Get an Organization",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "Organization Id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/controllers.OrganizationCreateRequest"
                        }
                    }
                }
            }
        }
    },
    "definitions": {
        "controllers.ComponentHealth": {
            "type": "object",
            "properties": {
                "detail": {
                    "type": "string"
                },
                "isHealthy": {
                    "type": "boolean"
                },
                "name": {
                    "type": "string"
                }
            }
        },
        "controllers.HealthResponse": {
            "type": "object",
            "properties": {
                "components": {
                    "type": "array",
                    "items": {
                        "$ref": "#/definitions/controllers.ComponentHealth"
                    }
                }
            }
        },
        "controllers.OrganizationCreateRequest": {
            "type": "object",
            "properties": {
                "name": {
                    "type": "string"
                }
            }
        },
        "user.AccountModel": {
            "type": "object",
            "properties": {
                "created": {
                    "type": "string"
                },
                "id": {
                    "type": "string"
                },
                "name": {
                    "type": "string"
                },
                "removed": {
                    "type": "boolean"
                },
                "updated": {
                    "type": "string"
                }
            }
        }
    }
}`

// SwaggerInfo holds exported Swagger Info so clients can modify it
var SwaggerInfo = &swag.Spec{
	Version:          "1.0",
	Host:             "",
	BasePath:         "",
	Schemes:          []string{},
	Title:            "Consent API",
	Description:      "",
	InfoInstanceName: "swagger",
	SwaggerTemplate:  docTemplate,
}

func init() {
	swag.Register(SwaggerInfo.InstanceName(), SwaggerInfo)
}
