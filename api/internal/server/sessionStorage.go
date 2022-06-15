package server

type sessionStorage interface {
	Get(sessionID string, key string) string
	Set(sessionID string, key string, value string)
}
