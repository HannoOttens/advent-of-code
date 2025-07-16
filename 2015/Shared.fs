namespace Shared

module Shared =
    let isPartA (argv : string array) = argv.Length < 2 || argv[1] <> "b"