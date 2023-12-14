package main

import "core:runtime"

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
			result[i] = a[i] + b[i] / a[i]
		} else {
			result[i] = a[i] / b[i] - b[i]
		}
	}
}
