package main

@export
add_arrays :: proc "c" (result, a, b: [^]f64, length: i32) {
	for i := i32(0); i < length; i += 1 {
		result[i] = a[i] + b[i]
	}
}