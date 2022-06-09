package context

type SessionStorage interface {
	Get(sessionID string, key string) string
	Set(sessionID string, key string, value string)
}
