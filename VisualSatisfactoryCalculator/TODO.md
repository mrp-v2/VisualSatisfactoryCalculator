1. Make numbers minimum rate based
    - [x] Normal connections
    - [ ] Fixed Rate connections
        - might need to change how multi-connections work
        - try delaying abnormal connection processing until after processing all reachable normal connections, then see which attached nodes haven't been updated and work from there
        - maybe have a popup happen to let the user decide what happens if there are multiple options the program could do
        - process multi-connections in order of complexity (how many nodes are connected), and in order of which ones are easy to adjust
2. Convert to Factorio
    - [ ] user inputs
        - [ ] machine tier
        - [ ] machine count interval (can specify # of machines must be a multiple of a number n)
        - [ ] max number of beacons per machine (for machines not at start or end)
        - [ ] beacon falloff behavior
            - [ ] edge beacon bonus (bonus for machines within interval of start or end)
            - [ ] edge bonus falloff rate (how fast the start or end bonus goes down with each interval farther from the start or end)
        - [ ] modules available
        - [ ] priority order of productivity, power consumption, and speed/machine count