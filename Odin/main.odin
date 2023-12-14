package main

import "core:runtime"
import "core:slice"

@export
bench1 :: proc "c" (result, a, b: [^]i32, length: i32) {
	for i := i32(0); i < length; i += 1 {
		if a[i] < b[i] {
			result[i] = a[i] + b[i]
		} else {
			result[i] = a[i] - b[i]
		}
	}
}

@export
bench2 :: proc "c" (result, a, b: [^]i32, length: i32) {
	for i := i32(0); i < length; i += 1 {
		if a[i] < b[i] {
			result[i] = a[i] + b[i] * a[i]
		} else {
			result[i] = a[i] * b[i] - b[i]
		}
	}
}

@export
bench3 :: proc "c" (#no_alias result, a, b: [^]i32, length: i32) {
	for i := i32(0); i < length; i += 1 {
		if a[i] < b[i] {
			result[i] = b[i] * a[i]
		} else {
			result[i] = a[i] * b[i]
		}
	}

	for i := i32(0); i < length; i += 1 {
		if a[i] < b[i] {
			result[i] = result[i] + a[i]
		} else {
			result[i] = result[i] - b[i]
		}
	}
}

@export
min :: proc "c" (a: []i32) -> i32 {
	context = runtime.default_context()
	return slice.min(a)
}
