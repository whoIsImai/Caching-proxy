# CLI Caching Proxy with ASP.NET

## Overview

This project is a CLI-based caching proxy built with ASP.NET. It forwards requests to an actual server and caches the responses. If the same request is made again within a short period, the proxy returns the cached response instead of forwarding the request to the server, improving performance and reducing redundant server load.

## Features

- Request Forwarding: Sends incoming requests to the target server and retrieves responses.

- Response Caching: Stores responses in Redis to serve repeated requests efficiently.

- Optimized Performance: Reduces redundant requests and improves response times.
