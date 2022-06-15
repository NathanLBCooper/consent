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
        "/v1/account/organization": {
            "post": {
                "summary": "Create an Organization",
                "parameters": [
                    {
                        "description": "Create organization",
                        "name": "organization",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/controllers.OrganizationCreateRequest"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/account.OrganizationModel"
                        }
                    }
                }
            }
        },
        "/v1/account/organization/{id}": {
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
                            "$ref": "#/definitions/account.OrganizationModel"
                        }
                    }
                }
            }
        },
        "/v1/account/user": {
            "post": {
                "summary": "Create a user",
                "parameters": [
                    {
                        "description": "Create user",
                        "name": "user",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/account.UserCreateRequest"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/account.UserModel"
                        }
                    }
                }
            }
        },
        "/v1/account/user/{id}": {
            "get": {
                "summary": "Get an user",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "User Id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/account.UserModel"
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
        "/v1/participant/{id}": {
            "get": {
                "summary": "Get an participant",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "User Id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/participant.ParticipantModel"
                        }
                    }
                }
            }
        }
    },
    "definitions": {
        "account.OrganizationMember": {
            "type": "object",
            "properties": {
                "role": {
                    "$ref": "#/definitions/account.Role"
                }
            }
        },
        "account.OrganizationModel": {
            "type": "object",
            "properties": {
                "created": {
                    "type": "string"
                },
                "id": {
                    "type": "string"
                },
                "members": {
                    "type": "object",
                    "additionalProperties": {
                        "$ref": "#/definitions/account.OrganizationMember"
                    }
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
        },
        "account.Role": {
            "type": "object"
        },
        "account.UserCreateRequest": {
            "type": "object",
            "properties": {
                "name": {
                    "type": "string"
                }
            }
        },
        "account.UserModel": {
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
        },
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
        "participant.Agreement": {
            "type": "object",
            "properties": {
                "accepted": {
                    "type": "boolean"
                },
                "acceptedTime": {
                    "type": "string"
                },
                "contractVersionId": {
                    "type": "string"
                },
                "permissionId": {
                    "type": "string"
                }
            }
        },
        "participant.ParticipantModel": {
            "type": "object",
            "properties": {
                "acceptedPermissionIds": {
                    "description": "read model. Calculated from AllAgreements",
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "allAgreements": {
                    "type": "array",
                    "items": {
                        "$ref": "#/definitions/participant.Agreement"
                    }
                },
                "created": {
                    "type": "string"
                },
                "externalId": {
                    "type": "string"
                },
                "id": {
                    "type": "string"
                },
                "organizationId": {
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
	Version:          "",
	Host:             "",
	BasePath:         "",
	Schemes:          []string{},
	Title:            "",
	Description:      "",
	InfoInstanceName: "swagger",
	SwaggerTemplate:  docTemplate,
}

func init() {
	swag.Register(SwaggerInfo.InstanceName(), SwaggerInfo)
}
