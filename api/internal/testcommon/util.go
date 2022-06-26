package testcommon

func Coalesce[T any](left *T, right T) T {
	if left == nil {
		return right
	}
	return *left
}

func NewPtr[T any](v T) *T {
	return &v
}
